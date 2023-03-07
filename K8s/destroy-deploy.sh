#!/bin/bash

kubectl delete deployment cars-depl
#Ingress loadbalancer
kubectl delete deployment ingress-nginx-controller -n ingress-nginx