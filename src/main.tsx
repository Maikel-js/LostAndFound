import React from 'react'
import ReactDOM from 'react-dom/client'
import { HashRouter as Router, Routes, Route } from 'react-router-dom'
import Layout from './components/Layout'
import PublicHome from './views/PublicHome'
import './global.css'
import { DataProvider } from './context/DataContext'
import ClaimItemPage from './views/ClaimItemPage'
import AdminDashboard from './views/AdminDashboard'
import LoginPage from './views/LoginPage'

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
    <React.StrictMode>
        <DataProvider>
            <Router>
                <Layout>
                    <Routes>
                        <Route path="/" element={<PublicHome />} />
                        <Route path="/claim/:id" element={<ClaimItemPage />} />
                        <Route path="/admin" element={<AdminDashboard />} />
                        <Route path="/login" element={<LoginPage />} />
                    </Routes>
                </Layout>
            </Router>
        </DataProvider>
    </React.StrictMode>,
)
