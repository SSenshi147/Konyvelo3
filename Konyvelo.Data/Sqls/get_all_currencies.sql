SELECT c.id, sum(t.total) Total, c.code
from currencies c
left join accounts a on c.id = a.currency_id
left join transactions t on a.id = t.account_id
group by c.id