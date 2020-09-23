import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Bem vindo a Localiza</h1>
        <p>Selecione a opção desejada, no meu superior</p>
      </div>
    );
  }
}
