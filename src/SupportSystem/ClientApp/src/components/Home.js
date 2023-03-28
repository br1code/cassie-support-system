import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <main>
                <h1>Welcome to CassieGames Support</h1>
                <section>
                    <h3>FAQ</h3>
                    <p>Before asking for help, make sure to take a look at our most frequently asked questions.</p>
                </section>
            </main>
        );
    }
}
