import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

function Tickets(props) {
    const [tickets, setTickets] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchTickets();
    }, []);

    // TODO: error handling?
    async function fetchTickets() {
        const response = await fetch('api/tickets');
        const data = await response.json();
        setTickets(data);
        setLoading(false);
    }

    function renderTicketsTable() {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Id</th>
                        <th>Created At</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {tickets.map(ticket =>
                        <tr key={ticket.id}>
                            <td><Link to={`/tickets/${ticket.id}`}>{ticket.title}</Link></td>
                            <td>{`#${ticket.id}`}</td>
                            <td>{ticket.createdAt}</td>
                            <td>{ticket.status}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    let contents = loading
        ? <p><em>Loading...</em></p>
        : renderTicketsTable();

    return (
        <section>
            <h1>My Tickets</h1>
            {contents}
        </section>
    );
}

export default Tickets;