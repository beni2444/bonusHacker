import requests
from bs4 import BeautifulSoup
import json
import time

class WebScraper:
    def __init__(self, base_url):
        """Initialize the scraper with a base URL"""
        self.base_url = base_url
        self.headers = {
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'
        }
    
    def fetch_page(self, url):
        """Fetch a webpage and return the response"""
        try:
            response = requests.get(url, headers=self.headers, timeout=10)
            response.raise_for_status()  # Raises an error for bad status codes
            return response
        except requests.RequestException as e:
            print(f"Error fetching {url}: {e}")
            return None
    
    def parse_html(self, html_content):
        """Parse HTML content using BeautifulSoup"""
        return BeautifulSoup(html_content, 'lxml')
    
    def extract_data(self, soup):
        """Extract specific data from the parsed HTML
        Customize this method based on what you want to scrape"""
        
        # Example: Extract all links
        links = []
        for link in soup.find_all('a', href=True):
            links.append({
                'text': link.get_text(strip=True),
                'url': link['href']
            })
        
        # Example: Extract all headings
        headings = []
        for heading in soup.find_all(['h1', 'h2', 'h3']):
            headings.append({
                'level': heading.name,
                'text': heading.get_text(strip=True)
            })
        
        return {
            'links': links,
            'headings': headings
        }
    
    def save_to_json(self, data, filename='output.json'):
        """Save scraped data to a JSON file"""
        with open(filename, 'w', encoding='utf-8') as f:
            json.dump(data, f, indent=2, ensure_ascii=False)
        print(f"Data saved to {filename}")
    
    def scrape(self, url):
        """Main scraping method that coordinates everything"""
        print(f"Scraping {url}...")
        
        # Fetch the page
        response = self.fetch_page(url)
        if not response:
            return None
        
        # Parse the HTML
        soup = self.parse_html(response.content)
        
        # Extract data
        data = self.extract_data(soup)
        
        # Be polite: wait between requests
        time.sleep(1)
        
        return data


# Example usage
if __name__ == "__main__":
    # Initialize scraper
    scraper = WebScraper("https://example.com")
    
    # Scrape a page
    data = scraper.scrape("https://example.com")
    
    if data:
        # Save the results
        scraper.save_to_json(data)
        
        # Print summary
        print(f"\nScraped {len(data['links'])} links")
        print(f"Scraped {len(data['headings'])} headings")