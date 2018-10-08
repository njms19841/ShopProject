using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * queryBlockUser返回结果
	 */
	public class QueryBlockUserReslut {
		// 返回码，200 为正常。
		[JsonProperty]
		int code;
		// 被封禁用户列表。
		[JsonProperty]
		List<BlockUsers> users;
		// 错误信息。
		[JsonProperty]
		String errorMessage;
		
		public QueryBlockUserReslut(int code, List<BlockUsers> users, String errorMessage) {
			this.code = code;
			this.users = users;
			this.errorMessage = errorMessage;
		}
		
		/**
		 * 设置code
		 *
		 */	
		public void setCode(int code) {
			this.code = code;
		}
		
		/**
		 * 获取code
		 *
		 * @return Integer
		 */
		public int getCode() {
			return code;
		}
		
		/**
		 * 设置users
		 *
		 */	
		public void setUsers(List<BlockUsers> users) {
			this.users = users;
		}
		
		/**
		 * 获取users
		 *
		 * @return List<BlockUsers>
		 */
		public List<BlockUsers> getUsers() {
			return users;
		}
		
		/**
		 * 设置errorMessage
		 *
		 */	
		public void setErrorMessage(String errorMessage) {
			this.errorMessage = errorMessage;
		}
		
		/**
		 * 获取errorMessage
		 *
		 * @return String
		 */
		public String getErrorMessage() {
			return errorMessage;
		}
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
