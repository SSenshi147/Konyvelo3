select c.id CurrencyId, sum(t.total) Total, c.code CurrencyCode, a.name Name, a.id Id
from accounts a
join currencies c on a.currency_id = c.id
left join transactions t on t.account_id = a.id
group by a.id