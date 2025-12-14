import requests
from bs4 import BeautifulSoup
import json
import time
from urllib.parse import urlparse, urljoin
from collections import deque
import pika
import json

#add abstraction for metadata with config per supermarket (also with day they change bonus)
#make code re-usable
#clean message before sending it (too long)

# Example usage
if __name__ == "__main__":
    connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
    channel = connection.channel()
    # Define start URL, extract domain, initialise data structires for vistsed, to visit and products urls
    start_url = "https://www.lidl.nl/p/afbakbroodjes/p10022241"
    domain = urlparse(start_url).netloc
    products_url = set()
    visited = set()
    queue = deque([start_url])
    max_pages = 1000
    metadata = ["ods-price__value", "ods-price__stroke-price", "heading__title", "availability", "product-id__number", "short-description"]

    # Visit each page in the queue, extract links and enque them
    while queue:

        if len(visited) >= max_pages:
            break

        url = queue.popleft()
        parsed_url = urlparse(url)
        path = parsed_url.path.rstrip("/")
        
        
        try:
            response = requests.get(url, timeout=5)
            html = response.text
        except requests.RequestException:
            continue

        time.sleep(1)

        soup = BeautifulSoup(html, "html.parser")
        links = soup.find_all('a', href=True)
        for a in links:
            href = a["href"] 
            full_url = urljoin(url, href)
            parsed = urlparse(full_url)

            if parsed.netloc == domain:
                if full_url not in visited:
                    visited.add(full_url)
                    queue.append(full_url)
        
        if path.startswith("/p"):
            products_url.add(url)
            product_metadata = {}
            for data in metadata: 
                section = soup.find(class_=data)
                text = section.get_text(separator=" ", strip=True) if section else None
                product_metadata[data] = text
            print(product_metadata)
            message = json.dumps(product_metadata)
            channel.queue_declare(queue=message)

    connection.close()

