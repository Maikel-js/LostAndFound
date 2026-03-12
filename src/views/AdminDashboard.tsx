import React, { useMemo, useState } from 'react';
import { useData } from '../context/DataContext';
import { Claim, Item } from '../types';

const AdminDashboard: React.FC = () => {
    const { items, claims, currentUser, createItem, approveClaim, rejectClaim, loading, error } = useData();
    const [activeTab, setActiveTab] = useState<'overview' | 'items' | 'claims'>('overview');
    const [selectedItem, setSelectedItem] = useState<Item | null>(null);

    const totalItems = items.length;
    const foundItems = items.filter(i => i.status === 'Found').length;
    const claimedItems = items.filter(i => i.status === 'Returned').length;
    const pendingClaims = claims.filter(c => c.status === 'Pending').length;

    const claimsByItem = useMemo(() => {
        const map = new Map<string, typeof claims>();
        for (const claim of claims) {
            const list = map.get(claim.itemId) || [];
            list.push(claim);
            map.set(claim.itemId, list);
        }
        return map;
    }, [claims]);

    if (!currentUser || currentUser.role !== 'Administrator') {
        return <div style={{ padding: '1rem', color: 'var(--color-danger)' }}>Access Denied. Admin only.</div>;
    }

    return (
        <div>
            <h2 style={{ marginBottom: '1.5rem', borderBottom: '2px solid var(--color-primary)', paddingBottom: '0.5rem' }}>
                Admin Dashboard
            </h2>

            <div style={{ display: 'flex', gap: '1rem', marginBottom: '2rem' }}>
                <button className={`btn ${activeTab === 'overview' ? 'btn-primary' : ''}`} onClick={() => setActiveTab('overview')}>
                    Overview
                </button>
                <button className={`btn ${activeTab === 'items' ? 'btn-primary' : ''}`} onClick={() => setActiveTab('items')}>
                    Inventory
                </button>
                <button className={`btn ${activeTab === 'claims' ? 'btn-primary' : ''}`} onClick={() => setActiveTab('claims')}>
                    Claims ({pendingClaims})
                </button>
            </div>

            {loading && <p className="text-muted">Cargando...</p>}
            {error && <p style={{ color: 'var(--color-danger)' }}>{error}</p>}

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
                        <p style={{ fontSize: '2rem', fontWeight: 'bold', color: 'var(--color-accent)' }}>{pendingClaims}</p>
                    </div>
                </div>
            )}

            {activeTab === 'items' && (
                <InventoryList
                    items={items}
                    onCreateItem={createItem}
                    onSelectItem={setSelectedItem}
                    selectedItem={selectedItem}
                    claimsByItem={claimsByItem}
                />
            )}

            {activeTab === 'claims' && (
                <ClaimsList
                    claims={claims}
                    onApprove={approveClaim}
                    onReject={rejectClaim}
                />
            )}
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

const InventoryList = ({
    items,
    onCreateItem,
    onSelectItem,
    selectedItem,
    claimsByItem
}: {
    items: Item[];
    onCreateItem: (item: Omit<Item, 'id' | 'status' | 'reporterId'>) => Promise<void>;
    onSelectItem: (item: Item | null) => void;
    selectedItem: Item | null;
    claimsByItem: Map<string, Claim[]>;
}) => {
    const [showAddForm, setShowAddForm] = useState(false);

    return (
        <div>
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '1rem' }}>
                <h3>All Items</h3>
                <button className="btn btn-primary" onClick={() => setShowAddForm(!showAddForm)}>
                    {showAddForm ? 'Cancel' : 'Add New Item'}
                </button>
            </div>

            {showAddForm && <AddItemForm onCreate={onCreateItem} onClose={() => setShowAddForm(false)} />}

            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                    <tr style={{ textAlign: 'left', borderBottom: '1px solid var(--color-border)' }}>
                        <th style={{ padding: '0.5rem' }}>Name</th>
                        <th style={{ padding: '0.5rem' }}>Category</th>
                        <th style={{ padding: '0.5rem' }}>Status</th>
                        <th style={{ padding: '0.5rem' }}>Date Found</th>
                        <th style={{ padding: '0.5rem' }}>Actions</th>
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
                                    backgroundColor: item.status === 'Found' ? '#dbeafe' : '#dcfce7',
                                    fontSize: '0.8rem'
                                }}>
                                    {item.status === 'Found' ? 'No reclamado' : 'Reclamado'}
                                </span>
                            </td>
                            <td style={{ padding: '0.5rem' }}>{new Date(item.dateFound).toLocaleDateString()}</td>
                            <td style={{ padding: '0.5rem' }}>
                                <button className="btn btn-accent" onClick={() => onSelectItem(item)}>
                                    Ver detalles
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {selectedItem && (
                <div style={{ marginTop: '2rem', padding: '1rem', border: '1px solid var(--color-border)', borderRadius: '8px' }}>
                    <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                        <h4>Detalle del objeto</h4>
                        <button className="btn" onClick={() => onSelectItem(null)}>Cerrar</button>
                    </div>
                    <p><strong>Nombre:</strong> {selectedItem.name}</p>
                    <p><strong>Descripción:</strong> {selectedItem.description}</p>
                    <p><strong>Categoría:</strong> {selectedItem.category}</p>
                    <p><strong>Ubicación:</strong> {selectedItem.locationFound}</p>
                    <p><strong>Fecha:</strong> {new Date(selectedItem.dateFound).toLocaleDateString()}</p>
                    <p><strong>Estado:</strong> {selectedItem.status === 'Found' ? 'No reclamado' : 'Reclamado'}</p>

                    <div style={{ marginTop: '1rem' }}>
                        <h5>Historial de reclamos</h5>
                        {(claimsByItem.get(selectedItem.id) || []).length === 0 ? (
                            <p className="text-muted">No hay reclamos para este objeto.</p>
                        ) : (
                            <ul>
                                {(claimsByItem.get(selectedItem.id) || []).map(claim => (
                                    <li key={claim.id}>
                                        {claim.userName} ({claim.userEmail}) - {claim.status} - {new Date(claim.dateSubmitted).toLocaleDateString()}
                                    </li>
                                ))}
                            </ul>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

const AddItemForm = ({ onCreate, onClose }: { onCreate: (item: Omit<Item, 'id' | 'status' | 'reporterId'>) => Promise<void>; onClose: () => void }) => {
    const [formData, setFormData] = useState({
        name: '',
        description: '',
        category: 'Other',
        locationFound: '',
        dateFound: new Date().toISOString().slice(0, 10),
        imageUrl: ''
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        await onCreate({
            name: formData.name,
            description: formData.description,
            category: formData.category as any,
            locationFound: formData.locationFound,
            dateFound: new Date(formData.dateFound).toISOString(),
            imageUrl: formData.imageUrl || undefined
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
                    value={formData.category} onChange={e => setFormData({ ...formData, category: e.target.value })}>
                    <option value="Electronics">Electronics</option>
                    <option value="Books">Books</option>
                    <option value="Clothing">Clothing</option>
                    <option value="Keys">Keys</option>
                    <option value="Other">Other</option>
                </select>
                <input required placeholder="Location Found" className="p-2 border rounded"
                    value={formData.locationFound} onChange={e => setFormData({ ...formData, locationFound: e.target.value })} />
                <input type="date" required className="p-2 border rounded"
                    value={formData.dateFound} onChange={e => setFormData({ ...formData, dateFound: e.target.value })} />
                <input placeholder="Image URL (optional)" className="p-2 border rounded"
                    value={formData.imageUrl} onChange={e => setFormData({ ...formData, imageUrl: e.target.value })} />
                <textarea required placeholder="Description" style={{ gridColumn: '1/-1' }} className="p-2 border rounded"
                    value={formData.description} onChange={e => setFormData({ ...formData, description: e.target.value })} />
            </div>
            <button type="submit" className="btn btn-primary" style={{ marginTop: '1rem' }}>Save Item</button>
        </form>
    );
};

const ClaimsList = ({
    claims,
    onApprove,
    onReject
}: {
    claims: Claim[];
    onApprove: (id: string) => Promise<void>;
    onReject: (id: string) => Promise<void>;
}) => {
    return (
        <div>
            <h3>Pending Claims</h3>
            {claims.length === 0 ? <p className="text-muted">No claims.</p> : (
                <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem', marginTop: '1rem' }}>
                    {claims.map(claim => (
                        <div key={claim.id} className="card" style={{ padding: '1rem', border: '1px solid var(--color-border)' }}>
                            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '0.5rem' }}>
                                <strong>Claim for: {claim.itemName}</strong>
                                <span style={{ fontSize: '0.8rem' }}>{new Date(claim.dateSubmitted).toLocaleDateString()}</span>
                            </div>
                            <div style={{ backgroundColor: '#f8fafc', padding: '0.5rem', marginBottom: '1rem' }}>
                                <p><strong>Claimant:</strong> {claim.userName} ({claim.userEmail})</p>
                                <p><strong>Description:</strong> {claim.featureDescription}</p>
                                <p><strong>Where lost:</strong> {claim.locationLost}</p>
                                <p><strong>When lost:</strong> {new Date(claim.timeLost).toLocaleDateString()}</p>
                            </div>
                            {claim.status === 'Pending' && (
                                <div style={{ display: 'flex', gap: '1rem' }}>
                                    <button className="btn btn-primary" onClick={() => onApprove(claim.id)}>Approve Claim</button>
                                    <button className="btn btn-accent" onClick={() => onReject(claim.id)}>Reject</button>
                                </div>
                            )}
                            {claim.status !== 'Pending' && <p>Status: {claim.status}</p>}
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default AdminDashboard;
