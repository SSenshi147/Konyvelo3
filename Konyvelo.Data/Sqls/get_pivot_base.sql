select c.id CurrencyId, a.id AccountId, t.category Category, sum(t.total) Total, c.code CurrencyCode
from transactions t
join accounts a on t.account_id = a.id
join currencies c on a.currency_id = c.id
where t.date >= date(@BeginDate) and t.date <= date(@EndDate)
group by Category, CurrencyCode
order by Category asc