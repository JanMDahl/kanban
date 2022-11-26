export interface Ticket {
    id: number;
    title: string;
    description: string;
    status: number;
    priority: number;
    createdAt: string;
    updatedAt: string;
    closedAt: string;
}

export interface Column {
    id: number;
    name: string;
    tickets: Ticket[];
}