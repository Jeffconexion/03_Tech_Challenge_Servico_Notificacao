# Tech Challenger - Serviço de Notificação

# Introdução

Ao cadastrar um novo usuário, uma mensagem de confirmação é enviada por e-mail. Pensando nesses cenários seria possível o envio de notificação de promoção, exlusão de conta, entre outros fatores.

# Serviço de Notificação

- **Função**: Este serviço é responsável por enviar notificações aos usuários ou responsáveis por contatos com base no adicionar, atualizar e excluir contato.
- **Processo**:
  - Consome mensagens da `fila-notificacoes`.
  - Com base nas informações da mensagem, determina a ação apropriada.
  - Envia emails ou outros tipos de notificação para os usuários ou responsáveis. As mensagens podem ser personalizadas com base no tipo ação.

# Tecnologias Utilizadas:

- **.NET 8**: Framework para construção da Minimal API.
- **C#**: Linguagem de programação usada no desenvolvimento do projeto.
- **RabbitMQ**: Broker para o gerenciamento das mensagens.
- **WorkerService**: Broker para o gerenciamento das mensagens.

# Documentação

- [Documentação da API](https://horse-neon-79c.notion.site/Documenta-o-da-API-04183b890d7c47cb89af4445d01d6678?pvs=4)
- [Documentação de Estilo para C#](https://horse-neon-79c.notion.site/Documenta-o-de-Estilo-para-C-de62b229fd01436a96f7a090b4d11e27?pvs=4)
- [Documentação dos Testes](https://horse-neon-79c.notion.site/Documenta-o-dos-Testes-a402a32a16a24b1b925dab83201e7d19?pvs=4)
- [Documentação de Banco de Dados](https://horse-neon-79c.notion.site/Documenta-o-de-Banco-de-Dados-6ba60c4c8533491a9d28da71f6b57c93?pvs=4)
- [Guia de Estrutura do Projeto](https://horse-neon-79c.notion.site/Guia-de-Estrutura-do-Projeto-fbfbc24c616d456bb56306cfda2c0bc9?pvs=4)
