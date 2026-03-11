import React from 'react'
import ReactDOM from 'react-dom/client'
import { HashRouter as Router, Routes, Route } from 'react-router-dom'
import Layout from './components/Layout'
import PublicHome from './components/PublicHome'
import './global.css'
import { DataProvider } from './context/DataContext'
import ClaimItemPage from './components/ClaimItemPage'
import AdminDashboard from './components/AdminDashboard'

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
    <React.StrictMode>
        <DataProvider>
            <Router>
                <Layout>
                    <Routes>
                        <Route path="/" element={<PublicHome />} />
                        <Route path="/claim/:id" element={<ClaimItemPage />} />
                        <Route path="/admin" element={<AdminDashboard />} />
                    </Routes>
                </Layout>
            </Router>
        </DataProvider>
    </React.StrictMode>,
)
