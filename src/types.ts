export type UserRole = 'Student' | 'Administrator';
export type ItemCategory = 'Electronics' | 'Clothing' | 'Books' | 'Keys' | 'Other';
export type ItemStatus = 'Found' | 'Returned' | 'Lost';
export type ClaimStatus = 'Pending' | 'Approved' | 'Rejected';

export interface User {
    id: string;
    firstName: string;
    lastName: string;
    role: UserRole;
    email: string;
    enrollmentNumber: string;
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
    reporterId: string;
}

export interface Claim {
    id: string;
    itemId: string;
    itemName: string;
    userId: string;
    userName: string;
    userEmail: string;
    status: ClaimStatus;
    dateSubmitted: string;
    dateResolved?: string;
    featureDescription: string;
    locationLost: string;
    timeLost: string;
}
