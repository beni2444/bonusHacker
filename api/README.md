# Consumer for the Page Scraped event that the scraper publishes

Send message like this through rabbitmq interfac:

```
{
  "messageType": [
    "urn:message:api:PageScraped"
  ],
  "message": {
    "url": "hello"
  }
}
```