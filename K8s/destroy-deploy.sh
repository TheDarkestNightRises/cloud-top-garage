#!/bin/bash

# Destroy CarService
cd CarService
./destroy-car.sh
cd ..

# Destroy Garage
cd Garage
./destroy-garage.sh
cd ..

# Destroy User
cd User
./destroy-user.sh
cd ..

# Destroy Environment
cd Environment
./destroy-environment.sh
cd ..

#Ingress loadbalancer
kubectl delete deployment ingress-nginx-controller -n ingress-nginx