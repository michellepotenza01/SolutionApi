SolutionApi -

Composição da Equipe:
•	Nome: Ana Carolina de Castro Gonçalves  -  RM554669
•	Nome: Luisa Danielle  -  RM555292
•	Nome: Michelle Marques Potenza  -  RM557702
2TDSPW

--LINK VIDEOS--
Vídeo .net: https://youtu.be/p9ul4wBLeEw

Vídeo Cloud: https://youtu.be/9TAHD43vQzY

Video PITCH: https://youtu.be/NdbNXGFoqHc


Este repositório contém a implementação de uma API RESTful em .NET 8.0 com foco no gerenciamento de abrigos, voluntários, avisos climáticos e pessoas, com um banco de dados Oracle para persistência de dados. A API foi projetada para ser utilizada em ambientes locais e na nuvem, com um processo de containerização usando Docker.

Estrutura do Projeto
A arquitetura do projeto foi organizada para uma aplicação web escalável, utilizando Entity Framework Core para comunicação com o banco de dados, Swagger para documentação da API, e Docker para criação de containers.

Tecnologias Usadas
.NET 8.0: Framework principal para desenvolvimento da API.

Oracle Database 21c: Banco de dados usado para persistir os dados da aplicação.

Entity Framework Core 8.0: ORM para interação com o banco de dados Oracle.

Swagger: Para documentação interativa da API.

Docker: Para containerização da aplicação e banco de dados.

Funcionalidades da API
A SolutionApi oferece as seguintes funcionalidades, implementadas por meio de controllers:

Controllers:
AbrigoController:

GET /api/abrigo: Retorna todos os abrigos registrados.

GET /api/abrigo/{nomeAbrigo}: Retorna um abrigo específico pelo nome.

POST /api/abrigo: Cria um novo abrigo.

PUT /api/abrigo/{nomeAbrigo}: Atualiza um abrigo existente.

DELETE /api/abrigo/{nomeAbrigo}: Deleta um abrigo específico.

AvisoController:

GET /api/aviso: Retorna todos os avisos registrados.

GET /api/aviso/{tipoAviso}: Retorna um aviso específico pelo tipo.

POST /api/aviso: Cria um novo aviso.

PUT /api/aviso/{tipoAviso}: Atualiza um aviso existente.

DELETE /api/aviso/{tipoAviso}: Deleta um aviso específico.

PessoaController:

GET /api/pessoa: Retorna todas as pessoas cadastradas.

GET /api/pessoa/{cpf}: Retorna uma pessoa específica pelo CPF.

POST /api/pessoa: Cadastra uma nova pessoa.

PUT /api/pessoa/{cpf}: Atualiza os dados de uma pessoa existente.

DELETE /api/pessoa/{cpf}: Deleta uma pessoa específica.

VoluntarioController:

GET /api/voluntario: Retorna todos os voluntários cadastrados.

GET /api/voluntario/{cpf}: Retorna um voluntário específico pelo CPF.

POST /api/voluntario: Cadastra um novo voluntário.

PUT /api/voluntario/{cpf}: Atualiza os dados de um voluntário existente.

DELETE /api/voluntario/{cpf}: Deleta um voluntário específico.

Estrutura do Banco de Dados
Relacionamentos:
Pessoa ↔ Voluntário: Relacionamento de 1:1, onde cada voluntário é associado a uma pessoa (baseado no CPF).

Voluntário ↔ Abrigo: Relacionamento de 1:N, onde um abrigo pode ter vários voluntários.

Migrações:
O projeto utiliza migrations do Entity Framework Core para gerenciar as alterações no banco de dados. 

Arquivos Docker
Dockerfile
O Dockerfile foi criado para containerizar a aplicação .NET e gerar uma imagem que pode ser usada para rodar a aplicação em qualquer ambiente.

Este Dockerfile inclui todos os passos necessários para construir e rodar a aplicação, com a instalação do .NET SDK, configuração do usuário, e instalação de ferramentas necessárias.

dockerfile
Copiar
Editar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /App

ENV PATH=$PATH:/root/.dotnet/tools
RUN dotnet tool install --global dotnet-ef

COPY . ./

RUN dotnet restore SolutionApi.sln

RUN dotnet publish SolutionApi.sln -c Release -o out

RUN groupadd -r appuser && useradd -r -g appuser -m appuser

USER appuser
ENV PATH=$PATH:/home/appuser/.dotnet/tools
RUN dotnet tool install --global dotnet-ef

USER root

WORKDIR /App/out

RUN chown -R appuser:appuser /App/out

EXPOSE 5000
EXPOSE 8000

USER appuser

ENTRYPOINT ["dotnet", "SolutionApi.dll"]
Observação: O Dockerfile foi usado de exemplo, mas está com alguns problemas de configuração para rodar 100% corretamente. O Docker está criando as imagens tanto para a aplicação .NET quanto para o banco de dados Oracle, porém a imagem da aplicação não está sendo executada totalmente.

docker-compose.yml
O arquivo docker-compose.yml é usado para orquestrar a execução do Docker, permitindo que tanto o banco de dados quanto a aplicação .NET sejam executados em containers separados.


Validações e Metadados
As validações e metadados são definidos tanto nos Models quanto nos DTOs para garantir que os dados inseridos estejam corretos. Por exemplo:

Modelos de Dados
Validação no Abrigo: As propriedades como NomeAbrigo, Bairro e Tamanho são obrigatórias e com restrições de formato, utilizando Data Annotations.

Modelo Pessoa: Tem validações como CPF único, idade válida e PCD (Pessoa com Deficiência) com valores restringidos a "Sim" ou "Não".

Modelos de Relacionamento: Relacionamentos entre Voluntário e Pessoa, Voluntário e Abrigo são configurados com chaves estrangeiras e regras de exclusão em cascata.

DTOs (Data Transfer Objects)
Os DTOs são utilizados para melhorar as saídas da API, fornecendo apenas os dados relevantes, sem expor diretamente os modelos do banco de dados. Cada controller, como AbrigoController, utiliza DTOs como resposta para formatar os dados de maneira mais eficiente.

Documentação OpenAPI (Swagger)
A API está documentada utilizando o Swagger e Swashbuckle. Isso facilita a visualização e teste das rotas da API diretamente do navegador. Com o Swagger configurado, é possível visualizar os endpoints, os tipos de retorno, e até testar as requisições diretamente na interface.

Cloud (Configuração de Nuvem)
Oracle Database na Nuvem
A solução foi projetada para usar Oracle Database na nuvem, o que possibilita a escalabilidade do banco de dados em serviços como Oracle Cloud ou Amazon RDS. A string de conexão no appsettings.json pode ser configurada para usar o banco de dados na nuvem. Caso precise configurar para diferentes ambientes, o Docker Compose também pode ser utilizado para conectar a aplicação a um banco na nuvem.

Conclusão
O projeto SolutionApi é uma API robusta e escalável, com uma boa estrutura de validação, documentação e integração com o Oracle Database. Ele é containerizado usando Docker e pode ser facilmente escalado para ambientes de nuvem. A implementação da Swagger UI permite uma integração e testes rápidos das funcionalidades da API, tornando a aplicação ainda mais eficiente e acessível para os desenvolvedores.
