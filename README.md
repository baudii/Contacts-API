# Contacts API
## Описание проекта

Contacts API — веб-приложение для управления контактами. Приложение позволяет выполнять CRUD операции с двумя связанными таблицами: Contact и Person. Реализация выполнена с использованием принципов Clean Architecture, что обеспечивает четкое разделение слоев и модульность кода.
## Основные возможности

- CRUD операции для сущностей Contact и Person
- Взаимодействие с API через Swagger UI для удобного тестирования
- Структура проекта организована в виде 4 отдельных проектов + модульные тесты

## Архитектура проекта

Проект разделен на несколько слоев:
- Contacts-API.Domain — модели данных и контекст базы данных.
- Contacts-API.Application — бизнес-логика приложения (CQRS, обработчики команд и запросов).
- Contacts-API.Infrastructure — репозитории и доступ к данным.
- Contacts-API.API — слой API, предоставляющий доступ к функционалу приложения через HTTP (Web API).
- Contacts-API.Tests — модульные тесты компонентов системы.

## Развертывание проекта

Для развертывания проекта в контейнере Docker выполните следующую команду из корневого каталога:

```
docker-compose up --build
```

Эта команда создаст контейнеры, инициализирует БД, заполнит ее тестовыми данными. 

> [!IMPORTANT]  
> После успешного запуска API будет доступен через `Swagger UI` для тестирования всех методов.

Инструменты и технологии

- ASP.NET Core Web API (.NET 8)
- Swagger UI для документирования и тестирования API
- PostgreSQL и EF Core для взаимодействия с БД
- Docker для контейнеризации
- MediatR для реализации CQRS
- XUnit, FluentAssertions, FakeItEasy для Unit-тестирования
