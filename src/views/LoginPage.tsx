import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useData } from '../context/DataContext';

const LoginPage: React.FC = () => {
    const { login, error } = useData();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const user = await login(email, password);
        navigate(user.role === 'Administrator' ? '/admin' : '/');
    };

    return (
        <div style={{ maxWidth: '480px', margin: '0 auto' }}>
            <h2 style={{ marginBottom: '1rem' }}>Iniciar sesión</h2>
            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
                <input
                    type="email"
                    required
                    placeholder="Email"
                    className="p-2 border rounded"
                    value={email}
                    onChange={e => setEmail(e.target.value)}
                />
                <input
                    type="password"
                    required
                    placeholder="Password"
                    className="p-2 border rounded"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                />
                <button className="btn btn-primary" type="submit">Login</button>
                {error && <p style={{ color: 'var(--color-danger)' }}>{error}</p>}
            </form>
            <p className="text-muted" style={{ marginTop: '1rem' }}>
                Admin default: admin@school.edu / Admin123!
            </p>
        </div>
    );
};

export default LoginPage;
