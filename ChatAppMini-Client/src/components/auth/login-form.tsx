"use client"
import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import axios from 'axios';
import Cookies from 'js-cookie';
import { 
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '@/components/ui/form';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { InteractiveHoverButton } from "@/components/magicui/interactive-hover-button";

const loginSchema = z.object({
  email: z.string().email('Invalid email format'),
  password: z.string().min(6, 'Password must be at least 6 characters'),
});

type LoginFormData = z.infer<typeof loginSchema>;

export function LoginForm() {
  const router = useRouter();
  const [error, setError] = useState('');
  
  const form = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: '',
      password: '',
    },
  });

  const onSubmit = async (data: LoginFormData) => {
    try {
      const response = await axios.post('http://localhost:5189/api/auth/login', data);

      if (response.data.status === 400) {
        setError(response.data.message || 'Login failed');
        return;
      }

      Cookies.set('accessToken', response.data.data.accessToken, { 
        expires: 1/24, 
        sameSite: 'strict', 
        path: "/" 
      });
      router.push('/chat');
      
    } catch (err: any) {
      setError('An error occurred during login');
    }
  };

  return (
    <div className="w-full max-w-md p-6 space-y-4 bg-white rounded-lg shadow-lg">
      <h2 className="text-2xl font-bold text-center">Login</h2>
      {error && <div className="p-3 text-sm text-red-500 bg-red-50 rounded">{error}</div>}
      
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
          <FormField
            control={form.control}
            name="email"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Email</FormLabel>
                <FormControl>
                  <Input 
                    {...field}
                    type="email"
                    placeholder="Enter your email"
                    className="w-full"
                  />
                </FormControl>
                <FormMessage>
                  {form.formState.errors.email && <span className="text-red-500">{form.formState.errors.email.message}</span>}
                </FormMessage>
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="password"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Password</FormLabel>
                <FormControl>
                  <Input 
                    {...field}
                    type="password"
                    placeholder="Enter your password"
                    className="w-full"
                  />
                </FormControl>
                <FormMessage>
                  {form.formState.errors.password && <span className="text-red-500">{form.formState.errors.password.message}</span>}
                </FormMessage>
              </FormItem>
            )}
          />
          <InteractiveHoverButton
            type="submit"
            disabled={form.formState.isSubmitting}
          >
            {form.formState.isSubmitting ? 'Logging in...' : 'Login'}
          </InteractiveHoverButton>
        </form>
      </Form>
    </div>
  );
}