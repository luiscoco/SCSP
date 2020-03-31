	  
      call setup(nc, hf, mixture_file, reference_state, error_code, error_message)
      if (error_code /= 0) then
        write (*,*) 'The following error occurred during REFPROP initialization:'
        write (*,*) error_message
        !stop
      end if
