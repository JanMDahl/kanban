import { Ticket } from './Ticket';
import App from './App';

export const getTickets = async () => {
    const response = await fetch(`https://localhost:7081/Ticket`);
    const tickets = await response.json();
    return tickets as Ticket[];
};

export const createTicket = async (ticket: Ticket) => {
    const response = await fetch(`https://localhost:7081/Ticket`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(ticket)
    });
    const newTicket = await response.json();
    return newTicket as Ticket;
}

export const editTicket = async (ticket: Ticket) => {
    const response = await fetch(`https://localhost:7081/Ticket/`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(ticket)
    });
    const updatedTicket = await response.json();
    return updatedTicket as Ticket;
}

export const deleteTicket = async (id: number) => {
    const response = await fetch(`https://localhost:7081/Ticket/${id}`, {
        method: 'DELETE'
    });
    return response;
}