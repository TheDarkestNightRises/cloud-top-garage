#!/bin/bash

#Deployment
kubectl delete deployment traefik-deployment
kubectl delete service traefik-dashboard-service
kubectl delete service traefik-web-service
