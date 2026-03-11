const API_BASE_URL = (import.meta.env.VITE_API_URL as string) || 'http://localhost:5000/api';

export const ApiService = {
    
    getToken: () => localStorage.getItem('jwt_token'),

    async fetchWithAuth(endpoint: string, options: RequestInit = {}) {
        const token = this.getToken();
        
        const headers = new Headers(options.headers || {});
        headers.append('Content-Type', 'application/json');
        
        if (token) {
            headers.append('Authorization', `Bearer ${token}`);
        }

        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            ...options,
            headers
        });

        if (response.status === 401) {
            console.error("No autorizado. Redirigiendo al login...");
        }

        return response;
    },

    async login(email: string, password: string) {
        const response = await fetch(`${API_BASE_URL}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });
        
        if (response.ok) {
            const data = await response.json();
            localStorage.setItem('jwt_token', data.token);
            return data;
        }
        throw new Error('Login fallido');
    },

    async getItems() {
        const response = await fetch(`${API_BASE_URL}/items`);
        return await response.json();
    },

    async createItem(itemData: any) {
        const response = await this.fetchWithAuth('/items', {
            method: 'POST',
            body: JSON.stringify(itemData)
        });
        
        return await response.json();
    }
};
