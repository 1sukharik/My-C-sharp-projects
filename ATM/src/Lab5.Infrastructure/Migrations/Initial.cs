using FluentMigrator;
using Itmo.Dev.Platform.Postgres.Migrations;

namespace Lab5.Infrastructure.Migrations;

[Migration(1, "Initial")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
    """
    create table if not exists users
    (
        user_id bigint primary key generated always as identity,
        user_name text not null,
        user_balance bigint not null,
        user_pin text not null
    );
    
    create table if not exists user_operations
    (
        operation_id bigint generated always as identity primary key,
        user_id bigint not null references users(user_id),
        user_type_of_operation text not null,
        user_operation_amount_of_money bigint not null,
        operation_timestamp timestamp default current_timestamp,
    
        foreign key (user_id) references users(user_id)
    );
    
    """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
    $"""
    drop table users;
    drop table user_operations;

    drop type user_operation;
    """;
}