import NudgeBackEnd.model_of_truth as truth
import sys
import numpy as np

#Recive passed arguments
inputs = sys.argv

#Take the arg from the passed argument
inputs = inputs[1]

#Convert string to array
inputs = eval('['+inputs+']')

#Convert array to numpy array
inputs = np.array(inputs, dtype='|S4')
inputs = inputs.astype(np.float)

#Find probability
prob = truth.Train().getProbailities(inputs=inputs)

#Save txt file with probability
output = open('prob.txt','w+')
output.write('%f'% prob)