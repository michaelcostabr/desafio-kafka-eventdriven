import React, { Component } from 'react';

export class Reservas extends Component {
    static displayName = Reservas.name;

    constructor(props) {
        super(props);
        this.state = { reservas: [], loading: true };
    }

    componentDidMount() {
        this.populateReservasData();
    }

    static renderReservasTable(reservas) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Localizador</th>
                        <th>Cliente</th>
                        <th>Carro</th>
                        <th>Status</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {reservas.map(reserva =>
                        <tr key={reserva.localizador}>
                            <td>{reserva.localizador}</td>
                            <td>{reserva.cliente}</td>
                            <td>{reserva.carro}</td>
                            <td>{reserva.status}</td>
                            <td>
                                <button disabled={reserva.status !== "PAGAMENTO APROVADO"}>Abrir Contrato</button>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Reservas.renderReservasTable(this.state.reservas);

        return (
            <div>
                <h1 id="tabelLabel" >Reservas</h1>
                <p>Lista de reservas para abertuda de contrato</p>
                {contents}
            </div>
        );
    }

    async populateReservasData() {
        const response = await fetch('reservas');
        const data = await response.json();
        this.setState({ reservas: data, loading: false });
    }
}
