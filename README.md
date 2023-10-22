# Vinícius Braga Freire - 2020054889

# Júnio Veras de Jesus Limas - 2020054617

## Twitter API

Este sistema é um clone de uma API simplificado da rede social Tweet. Nesta aplicação, simulamos 2 serviços básicos desta rede social: os usuários e os Tweets (objetos de comunicação utilizados pelos usuários para interagirem entre si).

Esta implementação em específico difere um pouco da rede social original, afinal não temos todas as operações disponíveis implementadas, além de que temos uma "rede completa". Isto significa que neste sistema todos usuários seguem todo mundo, ou seja, a timeline é composta pelos tweets de todos os usuários.

### Endpoints

Para os usuários que irão consumir desta API, iremos disponibilizar os seguintes endpoints:

#### Usuário:

Acesse através da rota `api/user`.

- `GET`: Permite retornar os usuários da rede através de consultas de arroba, nome de usuário, indentificador do usuário ou retorno de todos os usuários registrados na rede.
- `POST`: Criação de um novo usuário.
- `PUT`: Permitir editar o perfil do usuário.
- `/likes/like:PUT`: Emitir que um usuário curtiu um tweet.
- `/likes/unlike:PUT`: Emitir que um usuário descurtiu um tweet.
- `DELETE`: Excluir um usuário.

#### Tweet:

Acesse através da rota `api/tweet`.

- `GET`: Permite retornar os tweets da rede através de consultas de arroba do autor do tweet, do indentificador do tweet ou retorno de todos os tweets registrados na rede.
- `/subtree:GET`: Obtem a árvore de replies de um dado tweet.
- `/timeline:GET`: Obtem a timeline, que é um vetor de tweets, onde cada um tem mais relevancia que outros, logo a ordem dos tweets importam (tweets mais importantes vem "mais cedo" na lista).
- `POST`: Posta um tweet para dado usuário.
- `DELETE`: Exclui um dado tweet.

Como é impossível editar tweets, não temos a opção de PUT.

## Tecnologias

Nesta aplicação usamos C# e .NET para implementarmos uma web API. Para a implementação do banco utilizamos o PostgreSQL, um banco livre para uso. Para intuido de aprendizado dos alunos, utilizamos da ferramenta Entity Framework para o aprendizado de ferramentas de ORM.