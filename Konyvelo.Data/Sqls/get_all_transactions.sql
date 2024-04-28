select c.id CurrencyId, a.id AccountId, t.id Id, t.category Category, t.date Date, t.info Info, t.total Total, a.name AccountName, c.code CurrencyCode
from transactions t
join accounts a on t.account_id = a.id
join currencies c on a.currency_id = c.id