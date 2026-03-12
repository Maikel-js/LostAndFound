import React, { ReactNode } from 'react';
import { NavLink } from 'react-router-dom';
import { useData } from '../context/DataContext';

interface LayoutProps {
    children: ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
    const { currentUser, logout } = useData();

    return (
        <div className="app-layout">
            <nav style={{
                backgroundColor: 'var(--color-primary)',
                color: 'white',
                padding: '1rem',
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center'
            }}>
                <div style={{ fontWeight: 'bold', fontSize: '1.2rem' }}>School Lost & Found</div>
                <div style={{ display: 'flex', gap: '1rem' }}>
                    <NavLink to="/" className="btn" style={{ color: 'white' }}>Home</NavLink>
                    {currentUser?.role === 'Administrator' && (
                        <NavLink to="/admin" className="btn" style={{ color: 'white' }}>Admin</NavLink>
                    )}
                    {!currentUser ? (
                        <NavLink to="/login" className="btn" style={{ color: 'white' }}>Login</NavLink>
                    ) : (
                        <button className="btn" style={{ color: 'white' }} onClick={logout}>
                            Logout
                        </button>
                    )}
                </div>
            </nav>
            <main style={{ padding: '2rem' }}>
                {children}
            </main>
        </div>
    );
};

export default Layout;
