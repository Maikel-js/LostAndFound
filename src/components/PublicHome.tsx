import React, { useState } from 'react';
import { useData } from '../context/DataContext';
import { Item } from '../types';

import { useNavigate } from 'react-router-dom';

const PublicHome: React.FC = () => {
    const { items } = useData();
    const [searchTerm, setSearchTerm] = useState('');
    const [categoryFilter, setCategoryFilter] = useState<string>('all');
    const navigate = useNavigate();

    const filteredItems = items.filter(item => {
        const matchesSearch = item.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
            item.description.toLowerCase().includes(searchTerm.toLowerCase());
        const matchesCategory = categoryFilter === 'all' || item.category === categoryFilter;
        const isFoundAndUnclaimed = item.status === 'found'; // Only show found items

        return matchesSearch && matchesCategory && isFoundAndUnclaimed;
    });

    return (
        <div>
            <div style={{ marginBottom: '2rem', textAlign: 'center' }}>
                <h2 style={{ marginBottom: '1rem' }}>Find Your Lost Item</h2>
                <div style={{ display: 'flex', justifyContent: 'center', gap: '1rem', flexWrap: 'wrap' }}>
                    <input
                        type="text"
                        placeholder="Search for items..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        style={{ padding: '0.5rem', borderRadius: '4px', border: '1px solid var(--color-border)', width: '300px' }}
                    />
                    <select
                        value={categoryFilter}
                        onChange={(e) => setCategoryFilter(e.target.value)}
                        style={{ padding: '0.5rem', borderRadius: '4px', border: '1px solid var(--color-border)' }}
                    >
                        <option value="all">All Categories</option>
                        <option value="electronics">Electronics</option>
                        <option value="clothing">Clothing</option>
                        <option value="books">Books</option>
                        <option value="keys">Keys</option>
                        <option value="other">Other</option>
                    </select>
                </div>
            </div>

            <div style={{
                display: 'grid',
                gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))',
                gap: '2rem'
            }}>
                {filteredItems.map(item => (
                    <div key={item.id} className="card" style={{
                        backgroundColor: 'var(--color-surface)',
                        padding: '1.5rem',
                        borderRadius: '8px',
                        boxShadow: 'var(--shadow-md)',
                        border: '1px solid var(--color-border)'
                    }}>
                        <h3 style={{ marginBottom: '0.5rem' }}>{item.name}</h3>
                        <span style={{
                            display: 'inline-block',
                            padding: '0.25rem 0.5rem',
                            backgroundColor: '#e2e8f0',
                            borderRadius: '4px',
                            fontSize: '0.8rem',
                            marginBottom: '0.5rem'
                        }}>
                            {item.category.toUpperCase()}
                        </span>
                        <p style={{ color: 'var(--color-text-muted)', marginBottom: '1rem' }}>{item.description}</p>
                        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                            <span style={{ fontSize: '0.8rem' }}>Found: {new Date(item.dateFound).toLocaleDateString()}</span>
                            <button className="btn btn-primary" onClick={() => navigate(`/claim/${item.id}`)}>
                                Claim This
                            </button>
                        </div>
                    </div>
                ))}
                {filteredItems.length === 0 && (
                    <p style={{ gridColumn: '1/-1', textAlign: 'center', color: 'var(--color-text-muted)' }}>
                        No items found matching criteria.
                    </p>
                )}
            </div>
        </div>
    );
};

export default PublicHome;
