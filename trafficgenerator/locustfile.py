from locust import HttpUser, FastHttpUser, task, constant
import random
import time

product_categories = [
    "Shoes", "Baby", "Games", "Computers", "Beauty", "Tools",
    "Electronics", "Sports", "Books", "Clothing", "Kids", "Music", 
    "Jewelry", "Automotive", "Garden", "Toys", "Outdoors", "Industrial",
    "Health", "Grocery", "Movies", "Home"
]

def pick_random_category():
    length = len(product_categories)
    mean = (length - 1) / 2
    stddev = length / (length / 2)
    index = int(round(random.normalvariate(mean, stddev)))
    clamped_index = max(0, min(length - 1, index))
    return product_categories[clamped_index]

class PlaygroundUser(HttpUser):
    # wait_time = constant(3)

    @task
    def get_products_by_category(self):
        while (True):
            category = pick_random_category()
            rest = self.client.get(f"/api/Product/category/{category}")
            time.sleep(4)
