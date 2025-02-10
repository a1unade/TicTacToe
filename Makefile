docker-rebuild:  
		docker-compose down --rmi all -v
		docker-compose up --build -d

docker:  
		docker-compose up --build