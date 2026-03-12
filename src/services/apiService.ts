const ENV_BASE_URL = import.meta.env.VITE_API_URL as string | undefined;
const HOST_BASE_URL = typeof window !== 'undefined' && window.location?.hostname
    ? `http://${window.location.hostname}:5241/api`
    : undefined;
const API_BASE_URLS = [
    ENV_BASE_URL,
    HOST_BASE_URL,
    'http://127.0.0.1:5241/api',
    'http://localhost:5241/api'
].filter(Boolean) as string[];
let activeBaseUrl = API_BASE_URLS[0];

type ApiError = {
    message: string;
};

async function parseJson<T>(response: Response): Promise<T> {
    const text = await response.text();
    if (!text) return null as T;
    return JSON.parse(text) as T;
}

async function request<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
    const token = localStorage.getItem('jwt_token');
    const headers = new Headers(options.headers || {});
    headers.set('Content-Type', 'application/json');
    if (token) {
        headers.set('Authorization', `Bearer ${token}`);
    }

    const method = (options.method || 'GET').toUpperCase();
    const bases = [activeBaseUrl, ...API_BASE_URLS.filter(base => base !== activeBaseUrl)];
    let lastError: any = null;

    for (const base of bases) {
        try {
            const response = await fetch(`${base}${endpoint}`, { ...options, headers });
            if (!response.ok) {
                const errorBody = await parseJson<any>(response).catch(() => null);
                const validationErrors = errorBody?.errors
                    ? Object.values(errorBody.errors).flat().join(' ')
                    : null;
                const message = validationErrors
                    || errorBody?.message
                    || `Error ${response.status}`;
                throw new Error(message);
            }
            activeBaseUrl = base;
            return parseJson<T>(response);
        } catch (err: any) {
            lastError = err;
            if (method !== 'GET') {
                break;
            }
        }
    }

    throw lastError || new Error('Failed to fetch');
}

export const ApiService = {
    getToken: () => localStorage.getItem('jwt_token'),
    clearToken: () => localStorage.removeItem('jwt_token'),

    login(email: string, password: string) {
        return request<{ token: string; user: any }>('/auth/login', {
            method: 'POST',
            body: JSON.stringify({ email, password })
        });
    },

    getItems() {
        return request<any[]>('/items');
    },

    getItem(id: string, includeClaims = false) {
        const query = includeClaims ? '?includeClaims=true' : '';
        return request<any>(`/items/${id}${query}`);
    },

    createItem(itemData: any) {
        return request<any>('/items', {
            method: 'POST',
            body: JSON.stringify(itemData)
        });
    },

    createClaim(payload: any) {
        return request<any>('/claims', {
            method: 'POST',
            body: JSON.stringify(payload)
        });
    },

    getClaims(params?: { itemId?: string; status?: string }) {
        const search = new URLSearchParams();
        if (params?.itemId) search.set('itemId', params.itemId);
        if (params?.status) search.set('status', params.status);
        const query = search.toString() ? `?${search}` : '';
        return request<any[]>(`/claims${query}`);
    },

    getMyClaims() {
        return request<any[]>('/claims/mine');
    },

    approveClaim(id: string) {
        return request<any>(`/claims/${id}/approve`, { method: 'PUT' });
    },

    rejectClaim(id: string) {
        return request<any>(`/claims/${id}/reject`, { method: 'PUT' });
    }
};
