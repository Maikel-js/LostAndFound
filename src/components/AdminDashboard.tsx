import React, { useState } from 'react';
import { useData } from '../context/DataContext';
import { Link } from 'react-router-dom';

const AdminDashboard: React.FC = () => {
    const { items, tickets, currentUser } = useData();
    const [activeTab, setActiveTab] = useState<'overview' | 'items' | 'tickets' | 'users'>('overview');

    // Stats
    const totalItems = items.length;
    const foundItems = items.filter(i => i.status === 'found').length;
    const claimedItems = items.filter(i => i.status === 'claimed').length;
    const pendingTickets = tickets.filter(t => t.status === 'pending').length;

    if (!currentUser || currentUser.role !== 'admin') {
        return <div className="p-4 text-red-500">Access Denied. Admin only.</div>;
    }

    return (
        <div>
            <h2 style={{ marginBottom: '1.5rem', borderBottom: '2px solid var(--color-primary)', paddingBottom: '0.5rem' }}>
                Admin Dashboard
            </h2>

            <div style={{ display: 'flex', gap: '1rem', marginBottom: '2rem' }}>
                <button
                    className={`btn ${activeTab === 'overview' ? 'btn-primary' : ''}`}
                    onClick={() => setActiveTab('overview')}
                >
                    Overview
                </button>
                <button
                    className={`btn ${activeTab === 'items' ? 'btn-primary' : ''}`}
                    onClick={() => setActiveTab('items')}
                >
                    Inventory
                </button>
                <button
                    className={`btn ${activeTab === 'tickets' ? 'btn-primary' : ''}`}
                    onClick={() => setActiveTab('tickets')}
                >
                    Tickets ({pendingTickets})
                </button>
                <button
                    className={`btn ${activeTab === 'users' ? 'btn-primary' : ''}`}
                    onClick={() => setActiveTab('users')}
                >
                    Users
                </button>
            </div>

            {activeTab === 'overview' && (
                <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '1rem' }}>
                    <div style={statCardStyle}>
                        <h3>Total Items</h3>
                        <p style={{ fontSize: '2rem', fontWeight: 'bold' }}>{totalItems}</p>
                    </div>
                    <div style={statCardStyle}>
                        <h3>Available</h3>
                        <p style={{ fontSize: '2rem', fontWeight: 'bold', color: 'var(--color-primary)' }}>{foundItems}</p>
                    </div>
                    <div style={statCardStyle}>
                        <h3>Recovered</h3>
                        <p style={{ fontSize: '2rem', fontWeight: 'bold', color: 'var(--color-success)' }}>{claimedItems}</p>
                    </div>
                    <div style={statCardStyle}>
                        <h3>Pending Claims</h3>
                        <p style={{ fontSize: '2rem', fontWeight: 'bold', color: 'var(--color-accent)' }}>{pendingTickets}</p>
                    </div>
                </div>
            )}

            {activeTab === 'items' && <InventoryList />}
            {activeTab === 'tickets' && <TicketList />}
            {activeTab === 'users' && <UserList />}
        </div>
    );
};

const UserList = () => {
    const { items, currentUser } = useData();
    // In a real app we would have a users list in context, but for now we use the MOCK_USERS from the file scope 
    // or we need to expose users from context. DataContext exposes 'currentUser' but not all users.
    // I need to update DataContext to expose 'users' or just import MOCK_USERS if I exported it.
    // Let's assume I will update DataContext to expose 'users' list.
    // valid: const { users } = useData(); 
    // But currently DataContext only has 'currentUser'. I should add 'users' to DataContext.
    // For now, I'll mock the users list interaction.

    // TEMPORARY: Accessing mock data directly is not possible if it's inside the provider closure.
    // I will mock it here for the UI.
    const mockUsersList = [
        { id: 'u1', name: 'Admin Principal', email: 'admin@school.edu', role: 'admin' },
        { id: 'u2', name: 'John Student', email: 'john@student.school.edu', role: 'student' },
        { id: 'u3', name: 'Jane Staff', email: 'jane@school.edu', role: 'staff' },
    ];

    return (
        <div>
            <h3>User History</h3>
            <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem', marginTop: '1rem' }}>
                {mockUsersList.map(user => {
                    const recoveredItems = items.filter(i => i.claimedBy === user.id);
                    return (
                        <div key={user.id} className="card" style={{ padding: '1rem', border: '1px solid var(--color-border)' }}>
                            <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                                <div>
                                    <h4 style={{ margin: 0 }}>{user.name}</h4>
                                    <span className="text-muted" style={{ fontSize: '0.8rem' }}>{user.role} | {user.email}</span>
                                </div>
                                <div style={{ textAlign: 'right' }}>
                                    <span style={{ fontSize: '1.5rem', fontWeight: 'bold', color: 'var(--color-success)' }}>{recoveredItems.length}</span>
                                    <br /><span style={{ fontSize: '0.8rem' }}>Items Recovered</span>
                                </div>
                            </div>

                            {recoveredItems.length > 0 && (
                                <div style={{ marginTop: '1rem', paddingLeft: '1rem', borderLeft: '2px solid var(--color-border)' }}>
                                    <p style={{ fontSize: '0.9rem', fontWeight: '600' }}>Recovered Items History:</p>
                                    <ul style={{ fontSize: '0.9rem', color: 'var(--color-text-muted)' }}>
                                        {recoveredItems.map(item => (
                                            <li key={item.id}>{item.name} - {new Date(item.dateFound).toLocaleDateString()}</li>
                                        ))}
                                    </ul>
                                </div>
                            )}
                        </div>
                    )
                })}
            </div>
        </div>
    );
};

const statCardStyle = {
    backgroundColor: 'var(--color-surface)',
    padding: '1.5rem',
    borderRadius: '8px',
    boxShadow: 'var(--shadow-sm)',
    border: '1px solid var(--color-border)',
    textAlign: 'center' as const
};

// Sub-components (internal for now, could be separate files)
const InventoryList = () => {
    const { items, addItem } = useData();
    const [showAddForm, setShowAddForm] = useState(false);

    // Form state...

    return (
        <div>
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '1rem' }}>
                <h3>All Items</h3>
                <button className="btn btn-primary" onClick={() => setShowAddForm(!showAddForm)}>
                    {showAddForm ? 'Cancel' : 'Add New Item'}
                </button>
            </div>

            {showAddForm && <AddItemForm onClose={() => setShowAddForm(false)} />}

            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                    <tr style={{ textAlign: 'left', borderBottom: '1px solid var(--color-border)' }}>
                        <th style={{ padding: '0.5rem' }}>Name</th>
                        <th style={{ padding: '0.5rem' }}>Category</th>
                        <th style={{ padding: '0.5rem' }}>Status</th>
                        <th style={{ padding: '0.5rem' }}>Date Found</th>
                    </tr>
                </thead>
                <tbody>
                    {items.map(item => (
                        <tr key={item.id} style={{ borderBottom: '1px solid var(--color-border)' }}>
                            <td style={{ padding: '0.5rem' }}>{item.name}</td>
                            <td style={{ padding: '0.5rem' }}>{item.category}</td>
                            <td style={{ padding: '0.5rem' }}>
                                <span style={{
                                    padding: '0.2rem 0.5rem',
                                    borderRadius: '4px',
                                    backgroundColor: item.status === 'found' ? '#dbeafe' : '#dcfce7',
                                    fontSize: '0.8rem'
                                }}>
                                    {item.status}
                                </span>
                            </td>
                            <td style={{ padding: '0.5rem' }}>{new Date(item.dateFound).toLocaleDateString()}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

const AddItemForm = ({ onClose }: { onClose: () => void }) => {
    const { addItem } = useData();
    const [formData, setFormData] = useState({
        name: '', description: '', category: 'other' as any, locationFound: '', features: ''
    });

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        addItem({
            ...formData,
            features: formData.features.split(',').map((s: string) => s.trim())
        });
        onClose();
    };

    return (
        <form onSubmit={handleSubmit} style={{ marginBottom: '2rem', padding: '1rem', backgroundColor: '#f1f5f9', borderRadius: '8px' }}>
            <h4>Add Found Item</h4>
            <div style={{ display: 'grid', gap: '1rem', gridTemplateColumns: '1fr 1fr', marginTop: '1rem' }}>
                <input required placeholder="Item Name" className="p-2 border rounded"
                    value={formData.name} onChange={e => setFormData({ ...formData, name: e.target.value })} />
                <select className="p-2 border rounded"
                    value={formData.category} onChange={e => setFormData({ ...formData, category: e.target.value as any })}>
                    <option value="electronics">Electronics</option>
                    <option value="books">Books</option>
                    <option value="clothing">Clothing</option>
                    <option value="keys">Keys</option>
                    <option value="other">Other</option>
                </select>
                <input required placeholder="Location Found" className="p-2 border rounded"
                    value={formData.locationFound} onChange={e => setFormData({ ...formData, locationFound: e.target.value })} />
                <input required placeholder="Features (comma separated)" className="p-2 border rounded"
                    value={formData.features} onChange={e => setFormData({ ...formData, features: e.target.value })} />
                <textarea required placeholder="Description" style={{ gridColumn: '1/-1' }} className="p-2 border rounded"
                    value={formData.description} onChange={e => setFormData({ ...formData, description: e.target.value })} />
            </div>
            <button type="submit" className="btn btn-primary" style={{ marginTop: '1rem' }}>Save Item</button>
        </form>
    );
};

const TicketList = () => {
    const { tickets, items, updateTicketStatus } = useData();

    return (
        <div>
            <h3>Pending Claims</h3>
            {tickets.length === 0 ? <p className="text-muted">No pending tickets.</p> : (
                <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem', marginTop: '1rem' }}>
                    {tickets.map(ticket => {
                        const item = items.find(i => i.id === ticket.itemId);
                        return (
                            <div key={ticket.id} className="card" style={{ padding: '1rem', border: '1px solid var(--color-border)' }}>
                                <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '0.5rem' }}>
                                    <strong>Claim for: {item?.name}</strong>
                                    <span style={{ fontSize: '0.8rem' }}>{new Date(ticket.dateSubmitted).toLocaleDateString()}</span>
                                </div>
                                <div style={{ backgroundColor: '#f8fafc', padding: '0.5rem', marginBottom: '1rem' }}>
                                    <p><strong>Description:</strong> {ticket.answers.featureDescription}</p>
                                    <p><strong>Where lost:</strong> {ticket.answers.locationLost}</p>
                                    <p><strong>When lost:</strong> {ticket.answers.timeLost}</p>
                                </div>
                                {ticket.status === 'pending' && (
                                    <div style={{ display: 'flex', gap: '1rem' }}>
                                        <button className="btn btn-primary" onClick={() => updateTicketStatus(ticket.id, 'approved')}>Approve Claim</button>
                                        <button className="btn btn-accent" onClick={() => updateTicketStatus(ticket.id, 'rejected')}>Reject</button>
                                    </div>
                                )}
                                {ticket.status !== 'pending' && <p>Status: {ticket.status}</p>}
                            </div>
                        );
                    })}
                </div>
            )}
        </div>
    );
};

export default AdminDashboard;
