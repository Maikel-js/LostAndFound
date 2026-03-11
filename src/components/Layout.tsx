import React, { ReactNode } from 'react';
// import { useData } from '../context/DataContext'; // Commented out to avoid linter noise until deps installed

// Mock Link for now if router not installed
const MockLink = ({ to, children, className }: any) => <a href={to} className={className}>{children}</a>;

interface LayoutProps {
    children: ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
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
                    <MockLink to="/" className="btn" style={{ color: 'white' }}>Home</MockLink>
                    <MockLink to="/admin" className="btn" style={{ color: 'white' }}>Admin</MockLink>
                </div>
            </nav>
            <main style={{ padding: '2rem' }}>
                {children}
            </main>
        </div>
    );
};

export default Layout;
