# DataApiSample
OAuth Endpoint and DataProvider Endpoint sample for sql gateway service

## how does it work
```flow
client=>start: Sql Client
conn=>inputoutput: connection string
sqlServer=>operation: Sql gateway service
login=>subroutine: your OAuth Endpoint
auth=>condition: authorize userid/pwd?
cmd=>inputoutput: sql command text
query=>subroutine: your DataProvider Endpoint
fail=>end: Sql Client invalid grant
success=>end: result table to Sql Client

client->conn->sqlServer(right)->login->auth
auth(yes)->cmd
auth(no)->fail
cmd->query(right)->success
```
