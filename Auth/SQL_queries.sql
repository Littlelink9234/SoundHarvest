--CREATE DATABASE auth;

CREATE TABLE users (
	Id SERIAL PRIMARY KEY, 
	Email VARCHAR(50) unique not null, 
	Password_hash VARCHAR(255) not null,
	Created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);