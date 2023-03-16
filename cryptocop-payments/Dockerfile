FROM python:3.10

LABEL maintainer="Elfar Snær Arnarson"
LABEL author="Elfar Snær Arnarson"


COPY . /app
WORKDIR /app
RUN pip install -r requirements.txt


CMD ["python", "-u", "paymentService.py"]