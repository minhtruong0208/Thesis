FROM python:3.10.11

WORKDIR /app

COPY app/requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY app/ .
COPY Data/ /Data
COPY Model/ /Model

CMD ["python", "main.py"]