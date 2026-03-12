import React, { createContext, useContext, useEffect, useMemo, useState, ReactNode } from 'react';
import { ApiService } from '../services/apiService';
import { User, Item, Claim } from '../types';

interface DataContextType {
    currentUser: User | null;
    items: Item[];
    claims: Claim[];
    loading: boolean;
    error: string | null;
    login: (email: string, password: string) => Promise<User>;
    logout: () => void;
    refreshItems: () => Promise<void>;
    refreshClaims: () => Promise<void>;
    createItem: (item: Omit<Item, 'id' | 'status' | 'reporterId'>) => Promise<void>;
    createClaim: (payload: { itemId: string; featureDescription: string; locationLost: string; timeLost: string }) => Promise<void>;
    approveClaim: (claimId: string) => Promise<void>;
    rejectClaim: (claimId: string) => Promise<void>;
}

const DataContext = createContext<DataContextType | undefined>(undefined);

export const DataProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [currentUser, setCurrentUser] = useState<User | null>(() => {
        const raw = localStorage.getItem('auth_user');
        return raw ? JSON.parse(raw) as User : null;
    });
    const [items, setItems] = useState<Item[]>([]);
    const [claims, setClaims] = useState<Claim[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const refreshItems = async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await ApiService.getItems();
            setItems(data);
        } catch (err: any) {
            setError(err.message || 'No se pudieron cargar los objetos.');
        } finally {
            setLoading(false);
        }
    };

    const refreshClaims = async () => {
        if (!currentUser || currentUser.role !== 'Administrator') {
            setClaims([]);
            return;
        }
        setLoading(true);
        setError(null);
        try {
            const data = await ApiService.getClaims();
            setClaims(data);
        } catch (err: any) {
            setError(err.message || 'No se pudieron cargar los reclamos.');
        } finally {
            setLoading(false);
        }
    };

    const login = async (email: string, password: string) => {
        setLoading(true);
        setError(null);
        try {
            const data = await ApiService.login(email, password);
            localStorage.setItem('jwt_token', data.token);
            localStorage.setItem('auth_user', JSON.stringify(data.user));
            setCurrentUser(data.user);
            return data.user as User;
        } catch (err: any) {
            setError(err.message || 'Login fallido');
            throw err;
        } finally {
            setLoading(false);
        }
    };

    const logout = () => {
        ApiService.clearToken();
        localStorage.removeItem('auth_user');
        setCurrentUser(null);
        setClaims([]);
    };

    const createItem = async (newItem: Omit<Item, 'id' | 'status' | 'reporterId'>) => {
        setLoading(true);
        setError(null);
        try {
            await ApiService.createItem(newItem);
            await refreshItems();
        } catch (err: any) {
            setError(err.message || 'No se pudo crear el objeto.');
            throw err;
        } finally {
            setLoading(false);
        }
    };

    const createClaim = async (payload: { itemId: string; featureDescription: string; locationLost: string; timeLost: string }) => {
        setLoading(true);
        setError(null);
        try {
            await ApiService.createClaim(payload);
            await refreshClaims();
        } catch (err: any) {
            setError(err.message || 'No se pudo enviar el reclamo.');
            throw err;
        } finally {
            setLoading(false);
        }
    };

    const approveClaim = async (claimId: string) => {
        setLoading(true);
        setError(null);
        try {
            await ApiService.approveClaim(claimId);
            await refreshItems();
            await refreshClaims();
        } catch (err: any) {
            setError(err.message || 'No se pudo aprobar el reclamo.');
            throw err;
        } finally {
            setLoading(false);
        }
    };

    const rejectClaim = async (claimId: string) => {
        setLoading(true);
        setError(null);
        try {
            await ApiService.rejectClaim(claimId);
            await refreshClaims();
        } catch (err: any) {
            setError(err.message || 'No se pudo rechazar el reclamo.');
            throw err;
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        refreshItems();
    }, []);

    useEffect(() => {
        refreshClaims();
    }, [currentUser?.role]);

    const value = useMemo(() => ({
        currentUser,
        items,
        claims,
        loading,
        error,
        login,
        logout,
        refreshItems,
        refreshClaims,
        createItem,
        createClaim,
        approveClaim,
        rejectClaim
    }), [currentUser, items, claims, loading, error]);

    return (
        <DataContext.Provider value={{
            ...value
        }}>
            {children}
        </DataContext.Provider>
    );
};

export const useData = () => {
    const context = useContext(DataContext);
    if (context === undefined) {
        throw new Error('useData must be used within a DataProvider');
    }
    return context;
};
