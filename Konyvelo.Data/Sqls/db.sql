create table if not exists 'currencies' ('id' INTEGER NOT NULL CONSTRAINT 'PK_currencies' PRIMARY KEY AUTOINCREMENT, 'code' TEXT NOT NULL);

CREATE TABLE if not exists 'accounts'
            ('id' INTEGER NOT NULL CONSTRAINT 'PK_accounts' PRIMARY KEY AUTOINCREMENT,
            'name' TEXT NOT NULL,
            'currency_id' INTEGER NOT NULL, CONSTRAINT 'FK_accounts_currencies_currency_id' FOREIGN KEY ('currency_id') REFERENCES 'currencies' ('id') ON DELETE CASCADE);

CREATE TABLE if not exists 'transactions' (
            'id' INTEGER NOT NULL CONSTRAINT 'PK_transactions' PRIMARY KEY AUTOINCREMENT,
            'category' TEXT NOT NULL,
            'info' TEXT NULL,
            'date' TEXT NOT NULL,
            'total' INTEGER NOT NULL,
            'account_id' INTEGER NOT NULL,
            CONSTRAINT 'FK_transactions_accounts_account_id' FOREIGN KEY ('account_id') REFERENCES 'accounts' ('id') ON DELETE CASCADE);