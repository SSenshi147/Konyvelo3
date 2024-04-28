select t.id, t.category, t.total, t.info, t.date, a.name AccountName, a.id AccountId, c.code CurrencyCode, c.id CurrencyId
from transactions t
inner join accounts a on t.account_id = a.id
inner join currencies c on a.currency_id = c.id;