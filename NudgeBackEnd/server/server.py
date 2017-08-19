import socket
import sys

commandport = 20

def startSocket():
    s = socket.socket()
    host = socket.gethostname()
    s.bind((host,commandport))
    s.listen(256)
    return s

def getData(s,):
    connection, client_address = s.accept()
    try:
        print >> sys.stderr, 'connection from', client_address

        # Receive the data in small chunks and retransmit it
        while True:
            data = connection.recv(4096)
            print >> sys.stderr, 'received "%s"' % data
            if data:
                print >> sys.stderr, 'sending data back to the client'
                connection.sendall(data)
            else:
                print >> sys.stderr, 'no more data from', client_address
                break

    finally:
        # Clean up the connection
        connection.close()


if __name__ == '__main__':
    s = startSocket()
    while True:
        # Wait for a connection
        print >> sys.stderr, 'waiting for a connection'
        getData(s)

