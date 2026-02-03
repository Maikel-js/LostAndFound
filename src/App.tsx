import { useState } from 'react'

function App() {
    const [count, setCount] = useState(0)

    return (
        <div className="container" style={{ paddingTop: '2rem' }}>
            <header style={{ marginBottom: '2rem', textAlign: 'center' }}>
                <h1 style={{ fontSize: '2.5rem', marginBottom: '0.5rem' }}>School Lost & Found</h1>
                <p className="text-muted">Desktop Management System</p>
            </header>

            <main style={{ display: 'grid', placeItems: 'center', height: '50vh', border: '2px dashed var(--color-border)', borderRadius: '12px' }}>
                <div style={{ textAlign: 'center' }}>
                    <p style={{ marginBottom: '1rem' }}>Application initialized successfully.</p>
                    <button className="btn btn-primary" onClick={() => setCount((count) => count + 1)}>
                        Test Counter: {count}
                    </button>
                </div>
            </main>
        </div>
    )
}

export default App
