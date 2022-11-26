import { useEffect, useState } from 'react'
import './App.css'
import { Ticket, Column } from './Ticket';
import { getTickets, createTicket, editTicket, deleteTicket } from './api';

function App() {
  const [Columns, setColumns] = useState<Column[]>([
    { id: 0, name: 'Open', tickets: [] },
    { id: 1, name: 'In Progress', tickets: [] },
    { id: 2, name: 'Waiting for feedback', tickets: [] },
    { id: 3, name: 'Acceptance testing', tickets: [] }
  ]);

  const [selectedTicket, setSelectedTicket] = useState<Ticket | undefined>(undefined);

  const [cursorPosition, setCursorPosition] = useState({ x: 0, y: 0 });

  useEffect(() => {
    getTickets().then((tickets) => {
      const columns: Column[] = [...Columns];
      columns.forEach(column => {
        column.tickets = [];
      });

      tickets.forEach((ticket: Ticket) => {
        columns[ticket.status].tickets.push(ticket);
      });
      setColumns(columns);
    });

  }, []);

  function setTicketPosition(n: number) {

    if (selectedTicket) {

      let selectedTicketElement = document.getElementById(selectedTicket.id.toString());

      if (selectedTicket.status !== n) {
        selectedTicket.status = n;

        setColumns(column => {
          let columns: Column[] = [...column];
          columns.forEach(column => {
            column.tickets = column.tickets.filter(ticket => ticket.id !== selectedTicket.id);
          });
          columns[n].tickets.push(selectedTicket);
          return columns;
        });


        editTicket(selectedTicket);


      }

      if (selectedTicketElement) {
        selectedTicketElement.style.position = 'relative';
        selectedTicketElement.style.left = '0px';
        selectedTicketElement.style.top = '0px';
        selectedTicketElement.style.pointerEvents = 'all';
        selectedTicketElement.style.opacity = '1';
      }

      setSelectedTicket(undefined);
    }
  }

  const priorityToString = (priority: number) => {
    switch (priority) {
      case 0:
        return 'Low';
      case 1:
        return 'Medium';
      case 2:
        return 'High';
      default:
        return 'Unknown';
    }
  }



  return (

    <div className="App" onMouseMove={(e: React.MouseEvent) => {
      moveTicket(e);
    }
    }>
      <div className="columncontainer">
        {Columns.map((column, index) => (
          <div className="column">
            <h4>{column.name}</h4>
            <div className="columncontent" onMouseUp={() => setTicketPosition(index)}>
              {column.tickets.map((ticket: Ticket) => (
                <div className='ticket' id={ticket.id.toString()} key={ticket.id} onMouseDown={(e: React.MouseEvent) => {
                  setSelectedTicket(ticket)
                }
                }>
                  <div className='title'>{ticket.title}</div>
                  <div className='priority'>{priorityToString(ticket.priority)}</div>
                  <div className='created'>{new Date(ticket.createdAt).toLocaleString()}</div>
                  <div className='id'>{ticket.id}</div>
                </div>
              ))}

            </div>
          </div>))}
        {selectedTicket && (
          <div className='ticket ghost' id={selectedTicket.id.toString()} key={selectedTicket.id} style={{ display: "none" }}>
            <div className='title'>{selectedTicket.title}</div>
            <div className='priority'>{priorityToString(selectedTicket.priority)}</div>
            <div className='created'>{new Date(selectedTicket.createdAt).toLocaleString()}</div>
            <div className='id'>{selectedTicket.id}</div>
          </div>
        )}
      </div>
    </div>
  )

  function moveTicket(e: React.MouseEvent) {
    setCursorPosition({ x: e.pageX, y: e.pageY });
    if (selectedTicket) {
      let selectedTicketElement = document.querySelector(".ghost") as HTMLElement;
      let invis = document.getElementById(selectedTicket.id.toString()) as HTMLElement;
      if (selectedTicketElement) {
        invis.style.opacity = "0";
        selectedTicketElement.style.display = "block";
        selectedTicketElement.style.position = 'fixed';
        selectedTicketElement.style.left = `${cursorPosition.x - (selectedTicketElement.offsetHeight / 2)}px`;
        selectedTicketElement.style.top = `${cursorPosition.y - (selectedTicketElement.offsetWidth / 2)}px`;
        selectedTicketElement.style.pointerEvents = 'none';
      }
    }

  }
}

export default App
