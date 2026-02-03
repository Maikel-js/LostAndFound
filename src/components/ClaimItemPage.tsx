import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useData } from '../context/DataContext';

const ClaimItemPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const { items, createTicket, currentUser } = useData();
    const navigate = useNavigate();

    const item = items.find(i => i.id === id);

    const [answers, setAnswers] = useState({
        featureDescription: '',
        locationLost: '',
        timeLost: ''
    });

    if (!item) return <div>Item not found</div>;

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!currentUser) return alert('Please login first');

        createTicket({
            itemId: item.id,
            userId: currentUser.id,
            answers
        });

        alert('Claim submitted successfully! Check status in profile.');
        navigate('/');
    };

    return (
        <div style={{ maxWidth: '600px', margin: '0 auto' }}>
            <h2>Claiming: {item.name}</h2>
            <p className="text-muted" style={{ marginBottom: '2rem' }}>Please answer the following questions to verify ownership.</p>

            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
                <div className="form-group">
                    <label style={{ display: 'block', marginBottom: '.5rem', fontWeight: 500 }}>Describe the item in detail (features not visible in photo):</label>
                    <textarea
                        required
                        value={answers.featureDescription}
                        onChange={e => setAnswers({ ...answers, featureDescription: e.target.value })}
                        style={{ width: '100%', padding: '.5rem', minHeight: '100px' }}
                    />
                </div>

                <div className="form-group">
                    <label style={{ display: 'block', marginBottom: '.5rem', fontWeight: 500 }}>Where did you lose it?</label>
                    <input
                        type="text"
                        required
                        value={answers.locationLost}
                        onChange={e => setAnswers({ ...answers, locationLost: e.target.value })}
                        style={{ width: '100%', padding: '.5rem' }}
                    />
                </div>

                <div className="form-group">
                    <label style={{ display: 'block', marginBottom: '.5rem', fontWeight: 500 }}>When did you lose it?</label>
                    <input
                        type="date"
                        required
                        value={answers.timeLost}
                        onChange={e => setAnswers({ ...answers, timeLost: e.target.value })}
                        style={{ width: '100%', padding: '.5rem' }}
                    />
                </div>

                <button type="submit" className="btn btn-primary" style={{ marginTop: '1rem' }}>Submit Claim</button>
            </form>
        </div>
    );
};

export default ClaimItemPage;
