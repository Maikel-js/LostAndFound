export type UserRole = 'student' | 'staff' | 'admin';
export type ItemCategory = 'electronics' | 'clothing' | 'books' | 'keys' | 'other';
export type ItemStatus = 'found' | 'claimed' | 'archived';
export type TicketStatus = 'pending' | 'approved' | 'rejected';

export interface User {
    id: string;
    name: string;
    role: UserRole;
    email: string;
    department?: string; // For staff
    studentId?: string; // For students
}

export interface Item {
    id: string;
    name: string;
    description: string;
    category: ItemCategory;
    dateFound: string; // ISO date
    locationFound: string;
    status: ItemStatus;
    imageUrl?: string;
    features: string[]; // Key identifiers e.g. "Blue cover", "Scratch on back"
    claimedBy?: string; // User ID
}

export interface Ticket {
    id: string;
    itemId: string;
    userId: string;
    status: TicketStatus;
    dateSubmitted: string;
    answers: {
        featureDescription: string;
        locationLost: string;
        timeLost: string;
    };
}
