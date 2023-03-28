import React, { useState } from 'react';
import { Container, Row, Col, Form, FormGroup, Input, Button, Label } from 'reactstrap';
import { useNavigate } from "react-router-dom";

function NewTicket(props) {
    const navigate = useNavigate();
    const [title, setTitle] = useState("");
    const [body, setBody] = useState("");

    // TODO: error handling
    const handleSubmit = async (event) => {
        event.preventDefault();

        const response = await fetch("/api/tickets", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ title, body }),
        });

        if (response.ok) {
            console.log("Ticket created successfully");
            const newTicketId = await response.json();
            navigate(`/tickets/${newTicketId}`);
        } else {
            console.log("Failed to create ticket");
            // hacer algo cuando falla la creación del ticket
        }
    };

    return (
        <Container>
            <Row>
                <Col>
                    <Form onSubmit={handleSubmit}>
                        <FormGroup>
                            <Label for="title">Title</Label>
                            <Input
                                type="text"
                                name="title"
                                id="title"
                                value={title}
                                onChange={(event) => setTitle(event.target.value)}
                                required
                            />
                        </FormGroup>
                        <FormGroup>
                            <Label for="body">Body</Label>
                            <Input
                                type="textarea"
                                name="body"
                                id="body"
                                value={body}
                                onChange={(event) => setBody(event.target.value)}
                                required
                            />
                        </FormGroup>
                        <Button color="primary" type="submit">
                            Submit
                        </Button>
                    </Form>
                </Col>
            </Row>
        </Container>
    );
}

export default NewTicket;