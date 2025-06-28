# Payment Gateway GraphQL API

Полнофункциональный GraphQL API для обработки платежей с поддержкой реальных обновлений через WebSocket.

## Основные возможности

-  Инициализация платежей с валидацией данных
-  Запросы информации о платежах с фильтрацией и сортировкой
-  Реал-тайм обновления статусов через GraphQL Subscriptions
-  JWT аутентификация и авторизация
-  Поддержка различных платежных методов (CreditCard, PayPal и др.)
-  Пагинация и сложные запросы к данным

## Стек технологий

- **Backend**: .NET 6, Hot Chocolate (GraphQL)
- **База данных**: SQLite (с возможностью легкой миграции на другие СУБД)
- **Транспорт**: HTTP/WebSocket для Subscriptions
- **Аутентификация**: JWT с ролевой моделью
- **Инструменты**: Entity Framework Core, DataLoader

## 🚀 Быстрый старт

### Предварительные требования

- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- IDE (Visual Studio 2022+, Rider или VS Code)

### Установка

1. Клонируйте репозиторий:
```bash
git clone https://github.com/yourusername/PaymentGateway.git
cd PaymentGateway
```
2.  Настройте базу данных:
```bash
cd PaymentGateway.Server
dotnet ef database update
```
3.  Запустите сервер:
```bash
dotnet run
```


Сервер будет доступен на http://localhost:5000/graphql

## Примеры запросов

1. Инициализация платежа
```graphql
mutation {
  initPayment(input: {
    amount: 99.99,
    currency: "USD",
    method: "CreditCard",
    description: "Покупка в интернет-магазине"
  }) {
    id
    status
    transactionId
    paymentRequest {
      amount
      currency
      method
    }
  }
}
```
**Ожидаемый ответ:**
```json
{
    "data": {
        "initPayment": {
            "id": "ecceb3c6-7d39-419d-9b30-b2cf26c51ca0",
            "status": "INITIATED",
            "transactionId": "txn_0a1ed1aefabd4040",
            "paymentRequest": {
                "amount": 99.99,
                "currency": "USD",
                "method": "CreditCard"
            }
        }
    }
}
```

2.  Получение списка платежей
```graphql
query {
  payments {
    nodes {
      id
      status
      transactionId
      processedAt
      paymentRequest {
        amount
        currency
        method
        createdAt
      }
    }
  }
}
```
**Ожидаемый ответ:**
```json
{
    "data": {
        "payments": {
            "nodes": [
                {
                    "id": "ecceb3c6-7d39-419d-9b30-b2cf26c51ca0",
                    "status": "INITIATED",
                    "transactionId": "txn_0a1ed1aefabd4040",
                    "processedAt": "2025-06-28T15:27:17.769Z",
                    "paymentRequest": {
                        "amount": 99.99,
                        "currency": "USD",
                        "method": "CreditCard",
                        "createdAt": "2025-06-28T15:27:17.769Z"
                    }
                },
                {
                    "id": "63a0d8d0-2a37-47bc-9b8d-29a8084eae50",
                    "status": "INITIATED",
                    "transactionId": "txn_69644d47e19f44b5",
                    "processedAt": "2025-06-28T15:04:32.484Z",
                    "paymentRequest": {
                        "amount": 100,
                        "currency": "USD",
                        "method": "CreditCard",
                        "createdAt": "2025-06-28T15:04:32.484Z"
                    }
                },
                {
                    "id": "eb5a554c-3ecb-4129-bc35-5d2f67227f67",
                    "status": "INITIATED",
                    "transactionId": "txn_2c4f3f64371f48e3",
                    "processedAt": "2025-06-28T15:04:01.667Z",
                    "paymentRequest": {
                        "amount": 100,
                        "currency": "USD",
                        "method": "CreditCard",
                        "createdAt": "2025-06-28T15:04:01.667Z"
                    }
                },
                {
                    "id": "4216477e-d805-4c46-bba3-939cf005fe52",
                    "status": "INITIATED",
                    "transactionId": "txn_95fe08c0fa0048d0",
                    "processedAt": "2025-06-27T20:56:01.387Z",
                    "paymentRequest": {
                        "amount": 100.5,
                        "currency": "USD",
                        "method": "CreditCard",
                        "createdAt": "2025-06-27T20:56:01.387Z"
                    }
                },
                {
                    "id": "95a4628a-7915-42ef-8c49-4ef0fc6a9649",
                    "status": "CONFIRMED",
                    "transactionId": "txn_6ce2991f1a7144b0",
                    "processedAt": "2025-06-27T20:53:23.087Z",
                    "paymentRequest": {
                        "amount": 150.99,
                        "currency": "USD",
                        "method": "CreditCard",
                        "createdAt": "2025-06-27T20:53:23.087Z"
                    }
                },
                {
                    "id": "f77facbe-569e-4100-9b00-0de1400c4847",
                    "status": "INITIATED",
                    "transactionId": "9a3eff6ef2c348f9",
                    "processedAt": "2025-06-27T20:46:08.238Z",
                    "paymentRequest": {
                        "amount": 100.5,
                        "currency": "USD",
                        "method": "CreditCard",
                        "createdAt": "2025-06-27T20:46:08.238Z"
                    }
                },
                {
                    "id": "ca77156a-57df-4ccc-938e-67f124a290ff",
                    "status": "INITIATED",
                    "transactionId": "74485cc3838f4a42",
                    "processedAt": "2025-06-27T20:45:57.238Z",
                    "paymentRequest": {
                        "amount": 100.5,
                        "currency": "USD",
                        "method": "CreditCard",
                        "createdAt": "2025-06-27T20:45:57.238Z"
                    }
                },
                {
                    "id": "fce91688-d44a-4b23-9088-e4fbf6c6bf1a",
                    "status": "INITIATED",
                    "transactionId": "9f960946c56141e9",
                    "processedAt": "2025-06-27T20:45:56.563Z",
                    "paymentRequest": {
                        "amount": 100.5,
                        "currency": "USD",
                        "method": "CreditCard",
                        "createdAt": "2025-06-27T20:45:56.563Z"
                    }
                },
                {
                    "id": "8df3b5c3-7f9b-402b-b1f9-b2a8f213543e",
                    "status": "INITIATED",
                    "transactionId": "98340f12977d40f6",
                    "processedAt": "2025-06-27T20:45:55.772Z",
                    "paymentRequest": {
                        "amount": 100.5,
                        "currency": "USD",
                        "method": "CreditCard",
                        "createdAt": "2025-06-27T20:45:55.772Z"
                    }
                },
                {
                    "id": "dd96e434-004a-45e8-b718-d8c6198ace7c",
                    "status": "INITIATED",
                    "transactionId": "bb9268af700346ea",
                    "processedAt": "2025-06-27T20:45:55.023Z",
                    "paymentRequest": {
                        "amount": 100.5,
                        "currency": "USD",
                        "method": "CreditCard",
                        "createdAt": "2025-06-27T20:45:55.023Z"
                    }
                }
            ]
        }
    }
}
```



