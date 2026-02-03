import React, { createContext, useContext, useState, ReactNode } from 'react';
import { User, Item, Ticket, ItemCategory, ItemStatus } from '../types';

interface DataContextType {
    currentUser: User | null;
    items: Item[];
    tickets: Ticket[];
    login: (email: string) => void;
    logout: () => void;
    addItem: (item: Omit<Item, 'id' | 'status' | 'dateFound'>) => void;
    createTicket: (ticket: Omit<Ticket, 'id' | 'status' | 'dateSubmitted'>) => void;
    updateTicketStatus: (ticketId: string, status: 'approved' | 'rejected') => void;
    claimItem: (itemId: string, userId: string) => void;
}

const DataContext = createContext<DataContextType | undefined>(undefined);

// MOCK DATA
const MOCK_USERS: User[] = [
    { id: 'u1', name: 'Admin Principal', email: 'admin@school.edu', role: 'admin' },
    { id: 'u2', name: 'John Student', email: 'john@student.school.edu', role: 'student', studentId: 'S12345' },
    { id: 'u3', name: 'Jane Staff', email: 'jane@school.edu', role: 'staff', department: 'Science' },
];

const MOCK_ITEMS: Item[] = [
    {
        id: 'i1',
        name: 'Blue Physics Textbook',
        description: 'Advanced Physics 11th Grade, slight wear on corners.',
        category: 'books',
        dateFound: new Date(Date.now() - 86400000 * 2).toISOString(), // 2 days ago
        locationFound: 'Library Table 4',
        status: 'found',
        features: ['Blue cover', 'Hardcover', 'Sticker on back'],
    },
    {
        id: 'i2',
        name: 'Silver iPhone 13',
        description: 'Found with a black case. Screen locked.',
        category: 'electronics',
        dateFound: new Date(Date.now() - 86400000 * 1).toISOString(),
        locationFound: 'Gym Locker Room',
        status: 'found',
        features: ['Black case', 'Cracked screen protector'],
    },
    {
        id: 'i3',
        name: 'Red Scarf',
        description: 'Wool scarf, red with white stripes.',
        category: 'clothing',
        dateFound: new Date(Date.now() - 86400000 * 5).toISOString(),
        locationFound: 'Cafeteria',
        status: 'claimed',
        features: ['Red', 'Wool', 'White stripes'],
    }
];

const MOCK_TICKETS: Ticket[] = [];

export const DataProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [currentUser, setCurrentUser] = useState<User | null>(MOCK_USERS[0]); // Default login as Admin for dev
    const [items, setItems] = useState<Item[]>(MOCK_ITEMS);
    const [tickets, setTickets] = useState<Ticket[]>(MOCK_TICKETS);

    const login = (email: string) => {
        const user = MOCK_USERS.find(u => u.email === email);
        if (user) setCurrentUser(user);
    };

    const logout = () => setCurrentUser(null);

    const addItem = (newItem: Omit<Item, 'id' | 'status' | 'dateFound'>) => {
        const item: Item = {
            ...newItem,
            id: Math.random().toString(36).substr(2, 9),
            status: 'found',
            dateFound: new Date().toISOString(),
        };
        setItems(prev => [item, ...prev]);
    };

    const createTicket = (newTicket: Omit<Ticket, 'id' | 'status' | 'dateSubmitted'>) => {
        const ticket: Ticket = {
            ...newTicket,
            id: Math.random().toString(36).substr(2, 9),
            status: 'pending',
            dateSubmitted: new Date().toISOString(),
        };
        setTickets(prev => [...prev, ticket]);
    };

    const updateTicketStatus = (ticketId: string, status: 'approved' | 'rejected') => {
        setTickets(prev => prev.map(t => t.id === ticketId ? { ...t, status } : t));

        // If approved, mark item as claimed
        if (status === 'approved') {
            const ticket = tickets.find(t => t.id === ticketId);
            if (ticket) {
                claimItem(ticket.itemId, ticket.userId);
            }
        }
    };

    const claimItem = (itemId: string, userId: string) => {
        setItems(prev => prev.map(i => i.id === itemId ? { ...i, status: 'claimed', claimedBy: userId } : i));
    };

    return (
        <DataContext.Provider value={{
            currentUser, items, tickets,
            login, logout, addItem, createTicket, updateTicketStatus, claimItem
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
