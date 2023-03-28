import React, { useState, useEffect, useCallback } from 'react';
import { Container, Row, Col, Form, FormGroup, Input, Button } from 'reactstrap';
import { useParams } from 'react-router-dom';
import './Ticket.css';

function Ticket(props) {
    const { ticketId } = useParams();
    const [ticket, setTicket] = useState(null);
    const [loading, setLoading] = useState(true);
    const [newComment, setNewComment] = useState('');

    const fetchTicket = useCallback(async () => {
        const response = await fetch(`api/tickets/${ticketId}`);
        const data = await response.json();
        setTicket(data);
        setLoading(false);
    }, [ticketId]);

    useEffect(() => {
        fetchTicket();
    }, [fetchTicket]);

    // TODO: error handling
    const postComment = async () => {
        const response = await fetch(`api/tickets/${ticketId}/comments`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ body: newComment })
        });
        if (response.ok) {
            console.log("Comment created successfully");
            // Aquí puedes actualizar el estado del ticket con el nuevo comentario
            await fetchTicket()
        } else {
            console.log("Failed to create comment");
        }
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        postComment();
        setNewComment('');
    };

    const handleCommentChange = (e) => {
        setNewComment(e.target.value);
    };

    const renderTicket = () => {
        return (
            <Container>
                <Row>
                    <Col>
                        <h1 className="font-weight-bold">{ticket.title}</h1>
                        <h5>User #{ticket.userId}</h5>
                        <small className="text-muted">
                            {ticket.createdAt}
                        </small>
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <p className='my-4'>{ticket.body}</p>
                        <hr />
                    </Col>
                </Row>

                {ticket.comments.map((comment) => (
                    <Row key={comment.id}>
                        <Col>
                            <h5>User #{comment.userId}</h5>
                            <small className="text-muted">
                                {comment.createdAt}
                            </small>
                            <p className='my-4'>{comment.body}</p>
                            <hr />
                        </Col>
                    </Row>
                ))}

                <Row>
                    <Col>
                        <Form>
                            <FormGroup>
                                <Input type="textarea" name="comment" id="comment"
                                    value={newComment} onChange={handleCommentChange}/>
                            </FormGroup>
                            <Button color="primary" onClick={handleSubmit}>Add Comment</Button>
                        </Form>
                    </Col>
                </Row>
            </Container>
        );
    }

    let contents = loading
        ? <p><em>Loading...</em></p>
        : renderTicket();

    return (
        <div>
            {contents}
        </div>
    );
}

export default Ticket;