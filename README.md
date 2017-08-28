# Gabriel-StoneWallet
Projeto de gerenciamento de carteira para processo de seleção da Stone.

A solution está dividida em 3 projetos, organizados da seguinte forma:
- StoneWalletLibrary é uma DLL que contém a lógica de negócio e conexão com banco de dados.
- StoneWalletService é uma Web API que expõe a DLL como um serviço e trata da autenticação do usuário.
- StoneWalletService.Tests é o projeto onde estão localizados os testes unitários referentes ao serviço.

## Entidades
O projeto utiliza 3 entidades básicas de negócio, definidas no projeto StoneWalletLibrary:

### Cardholder
O dono do cartão de crédito. Um cardholder possui somente uma carteira e vários cartões.

#### Campos
- Name = Nome da pessoa.
- NationalIdNumber = Número nacional de identificação, exemplo: CPF ou SSN.
- Email = Email da pessoa, utilizado pelo serviço para ligar um cardholder a uma conta de usuário.
- Cards = Lista de cartões que pertencem a este cardholder.
- Wallet = Carteira associada a este cardholder.

### Card
O cartão de crédito. Um cartão pertence a somente um cardholder e pode pertencer a somente uma carteira.

#### Campos
- Number = Número do cartão.
- CVV = CVV do cartão.
- DueDate = Dia do mês onde será efetuado o pagamento do cartão.
- ExpirationDate = Data de vencimento do cartão.
- Limit = Limite do cartão.
- Credit = Crédito disponível.
- Wallet = A carteira a qual o cartão está associado.
- Cardholder = O cardholder a qual o cartão está associado.

### Wallet
A carteira onde são armazenados os cartões. A carteira pertence a somente um cardholder e possui vários cartões.

#### Campos
- UserLimit = Limite máximo de crédito definido pelo usuário.
- MaximumLimit = Limite máximo de crédito total.
- Credit = Crédito disponível.
- Cards = Lista de cartões que pertencem a esta carteira.
- Cardholder = Cardholder associado a esta carteira.

## Detalhamento dos Projetos
Os 3 projetos descritos acima estão subdivididos da seguinte forma:

### StoneWalletLibrary
- Models = As entidades de negócio descritas acima.
- Data = Classes de repositório que isolam a lógica de banco de dados.
- BusinessLogic = Classes que definem toda a lógica de negócio do projeto.

### StoneWalletService
- Controllers = Controladores Web API, um para cada classe de negócio.
- ViewModels = Define os ViewModels que serão utilizados pelos controllers.
- Além disso, várias classes foram auto geradas para tratar da autenticação, inicialização e criação da página web padrão de um projeto Web API

### StoneWalletService.Tests
- ControllerTests = Testes referentes aos controladores do serviço.
- MockRepositories = Classes de repositório utilizadas para substituir as classes de repositório da StoneWalletLibrary durante a execução dos testes

## Como Utilizar
Primeiramente, o usuário deve estar autenticado. Isso é feito através dos seguintes métodos padrão:
### /api/Account/Register/
- Método Post que recebe como argumentos no corpo da mensagem: Email, Password e ConfirmPassword. Cria uma conta de usuário.

### /Token
- Método Post que recebe como argumentos no corpo da mensagem: grant_type (que deverá ter o valor: password), username e password. Retorna um token de autenticação.

Em seguida, é necessário criar um cardholder para associar a este usuário. Isso é feito através do método:
### /api/Wallet/CreateCardholder
- Método Post que recebe como argumento no corpo da mensagem um objeto do tipo ViewModel.Cardholder. Somente são necessários os campos Name e NationalIdNumber, o Email será preenchido automaticamente com o email do usuário autenticado.

Após criar um cardholder, poderam ser criados uma carteira e cartões associados a este cardholder através dos métodos:
### /api/Card/CreateCard
- Método Post que recebe como argumento no corpo da mensagem um objeto do tipo ViewModel.Card. O campo Cardholder será preenchido automaticamente com o cardholder autenticado.

### /api/Wallet/CreateWallet
- Método Post que não recebe argumentos e cria uma carteira padrão vazia associada ao cardholder autenticado.

Com uma carteira e cartões criados, associe os cartões a carteira com o método:
### /api/Wallet/AddCardToWallet
- Método Put que recebe como argumento na querystring a Id do cartão que será associado a carteira do cardholder autenticado.

Você pode consultar a Id de um cartão através do método:
### /api/Cardholder/GetCardholder
- Método Get que retorna todas as informações do Cardholder associado ao usuário autenticado.

Você pode configurar o limite da carteira através do método:
### /api/Wallet/SetUserDefinedLimit?value=10000
- Método Put que recebe um valor na querystring que define o Limite permitido para gastos na carteira do cardholder autenticado.

Você pode pagar um cartão utilizando o método:
### /api/Card/PayCredit
- Método Post que recebe uma Id de um cartão e o valor a ser pago em cima do crédito do cartão.

E por fim você pode executar uma compra através da carteira com o método:
### /api/Wallet/ExecutePurchase
- Método Post que recebe um valor na querystring que será pago pela carteira seguindo a lógica definida no enunciado.

Estes não são os únicos métodos disponíveis no serviço, mas são os principais. Em relação aos métodos Delete, está sendo feita a exclusão lógica através de um atributo Deleted, nada é de fato apagado.

## Considerações
Estou entregando o projeto como uma solution do Visual Studio, pois não sei como fazer o processo de publish de um serviço Web Api com Code First sendo utilizado para a criação do banco de dados. Os testes unitários também não ficaram prontos, pois não sei como mockar um DBContext e sem isso só posso fazer testes muito básicos.

### Melhorias Possíveis
- Publicar o projeto.
- Completar os testes unitários.
- O tratamento de exceções foi feito da forma mais simples possível, poderia ser melhorado.
- A entidade Cardholder poderia estar associada a uma entidade "Pessoa" que guardaria uma lista de endereços, telefones, emails, etc.
- O processo de compra poderia armazenar um histórico de transações numa entidade "Transação" ao invés de simplesmente subtrair o valor.
